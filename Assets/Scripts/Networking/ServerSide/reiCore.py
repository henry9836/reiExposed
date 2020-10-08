from _thread import *
import mysql.connector
import socket
import enum
import numpy as np

#init
dbUsername = ""
dbPassword = ""
dbTable = ""
HOST = '' #All interfaces
PORT = 27100
MAXRECV = 8192 # 4*2048 (UTF-8 = 4 bytes each char)
SEPERATOR = "--"
USERSEPERATOR = "#"
ERROR_GENERAL = -1

#CONNECTION INFO
class PACKET(enum.Enum): 
    ACK = 0
    PACKAGE_SEND = 1
    PACKAGE_RECIEVE = 2
    REQUEST_LEADERBOARD = 3
    REQUEST_USERRANK = 4

#Get Hash Value
def getHash(name, time, item1, item2, item3, curr, msg):
	print("C BUILD")
	challenge = (str(name) + str(time) + str(curr) + str(msg) + str(item1) + str(item2) + str(item3))
	
	print("NUM BUILD")
	hash = np.uint64(1)
	mod = np.uint64(2147483647)
	mul = np.uint64(99643)

	chunksize = 1
	
	print("ENCODE BUILD")
	bytesArray = challenge.encode()

	print(bytes)
	
	print("FOR BUILD")
	for i in range(len(bytesArray)):
		if i % chunksize == 0:
			hash += np.uint64(str(bytesArray[i])) * mul
	
	print("Ret magic")
	return str(hash % mod)

class packetStruct:

	def __init__(self, input, db):
		try:
			self.data = input.split(SEPERATOR)
			#determine type
			try:
				self.type = int(self.data[0])
			except:
				print("Invalid Packet Type")
				self.type = ERROR_GENERAL #error value
				return;

			print(self.data)

			#setup packet depending on type of packet
			if self.type == PACKET.PACKAGE_SEND.value:
				#grab character blacklist
				blacklistFile = open("blacklist.txt", 'r')
				blacklist = blacklistFile.read().splitlines()
				blacklistFile.close()

				#check for naughtyness
				for i in range(1, len(self.data)):
					for y in blacklist:
						tmp = str(self.data[i]).lower()
						y = y.lower()
						if y in tmp:
							print("Invalid Word Found: " + y)
							self.type = ERROR_GENERAL #error value
							return;
						elif tmp == '':
							print("Null Value Found!")
							self.type = ERROR_GENERAL #error value
							return;

				#assign values
				print("ID")
				self.ID = str(self.data[1])
				print("msg")
				self.msg = str(self.data[2])
				print("curr")
				self.curr = int(self.data[3])
				print("item1")
				self.item1 = int(self.data[4])
				print("item2")
				self.item2 = int(self.data[5])
				print("item3")
				self.item3 = int(self.data[6])
				print("name")
				self.name = str(self.data[7])
				print("time")
				self.time = float(self.data[8])
				print("magic")
				self.magic = str(self.data[9])

				#check for incorrect values
				if (self.curr < 10):
					print("Invalid Amount Of Currency")
					self.type = ERROR_GENERAL #error value
					return;
				elif (self.item1 < 0):
					print("Item1 Amount Invalid")
					self.type = ERROR_GENERAL #error value
					return;
				elif (self.item2 < 0):
					print("Item2 Amount Invalid")
					self.type = ERROR_GENERAL #error value
					return;
				elif (self.item3 < 0):
					print("Item3 Amount Invalid")
					self.type = ERROR_GENERAL #error value
					return;
				elif (len(self.name) > 30):
					print("Name Too Long")
					self.type = ERROR_GENERAL #error value
					return;
				elif (self.time < 0):
					print("Invalid Time")
					self.type = ERROR_GENERAL #error value
					return;
				elif (len(self.msg) > 230):
					print("Msg Too Long")
					self.type = ERROR_GENERAL #error value
					return;

				#Check Hash
				print("Doign the maigc test")
				self.challenge = getHash(self.name, self.time, self.item1, self.item2, self.item3, self.curr, self.msg)
				print("Done.")

				if (self.challenge == self.magic):
					print("Magic matches!")
				else:
					print("Bad Magic")
					self.type = ERROR_GENERAL #error value
					return;
			elif self.type == PACKET.REQUEST_LEADERBOARD.value or self.type == PACKET.REQUEST_USERRANK.value:
				#grab character blacklist
				blacklistFile = open("blacklist.txt", 'r')
				blacklist = blacklistFile.read().splitlines()
				blacklistFile.close()

				#check for naughtyness
				for i in range(1, len(self.data)):
					for y in blacklist:
						tmp = str(self.data[i]).lower()
						y = y.lower()
						if y in tmp:
							print("Invalid Word Found: " + y)
							self.type = ERROR_GENERAL #error value
							return;
						elif tmp == '':
							print("Null Value Found!")
							self.type = ERROR_GENERAL #error value
							return;
				if (self.type == PACKET.REQUEST_LEADERBOARD.value):
					self.chunksize = int(self.data[1])
					self.offset = int(self.data[2])
					if (self.chunksize <= 0):
						self.chunksize = 1
					elif (self.chunksize > 10):
						self.chunksize = 10
				elif (self.type == PACKET.REQUEST_USERRANK.value):
					self.nameOfUser = str(self.data[1])
			#Nothing Values
			elif self.type == PACKET.ACK.value or self.type == PACKET.PACKAGE_RECIEVE.value:
				pass
			else:
				print("Unknown Packet Type {" + str(self.type) + "}")
				self.type = ERROR_GENERAL #error value
				return;
		except:
			print("Invalid Packet Structure")
			self.type = ERROR_GENERAL #error value
			return;

#CREATE A ENTRY
def createPackage(packet, _cursor, _db):
	#INSERT into Packages (ID, MSG, ATTACH1, ATTACH2, ATTACH3, NAME, TIME) VALUES ("FUZZER", "TEST", 1, 1, 1, "FUZZERNOTREALLY", "01:23:45.678900")
	print("Creating Package...")
	q = "INSERT INTO Packages (ID, MSG, CURR, ATTACH1, ATTACH2, ATTACH3, NAME, TIME) VALUES (%s, %s, %s, %s, %s, %s, %s, %s)"
	v = (packet.ID, packet.msg, packet.curr, packet.item1, packet.item2, packet.item3, packet.name, packet.time);
	_cursor.execute(q, v)
	#save changes
	_db.commit()
	print("Created Package")

#GET A RANDOM ENTRY
def getPackage(_cursor):
	q = "SELECT * FROM Packages ORDER BY RAND() LIMIT 1"
	_cursor.execute(q)
	return _cursor

def getRankChunk(_cursor, CHUNKSIZE, OFFSET):
	q = "SELECT ROW_NUMBER() OVER(ORDER BY TIME ASC) AS Ranking, TIME, NAME FROM Packages ORDER BY Ranking ASC LIMIT " + str(CHUNKSIZE) + " OFFSET " + str(OFFSET)
	_cursor.execute(q)
	return _cursor

def getUserRank(_cursor, NAME):
	q = "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY TIME ASC) AS Ranking, TIME, NAME FROM Packages)T WHERE NAME LIKE CONCAT('" + str(NAME) + "')"
	_cursor.execute(q)
	return _cursor

def decodeLeaderBoardPacket(element):

	rank = element[0]
	time = element[1]
	name = element[2]

	return str(rank) + USERSEPERATOR + str(time) + USERSEPERATOR + str(name)

def clientThread(conn):
	#Connect To DB
	print("Thread Connecting To Database...")

	db = mysql.connector.connect(host="localhost",user=dbUsername, password=dbPassword, database=dbTable, charset='utf8', use_unicode=True)

	print("Connected To Database!")

	#create cursor
	cursor = db.cursor()

	data = conn.recv(MAXRECV)

	data = data.decode('utf-8', 'replace').rstrip('\n')

	print(data)

	#Decode Packet
	packet = packetStruct(data, db)

	#Is Valid Packet?
	try:
		if packet.type >= 0:
			if packet.type == PACKET.ACK.value:
				print("ACK")
				conn.send(("0"+SEPERATOR+"ACK").encode('utf-8', 'replace'))
			elif packet.type == PACKET.PACKAGE_SEND.value:
				print("PACKAGE_SEND")
				createPackage(packet, cursor, db);
				conn.send(("0"+SEPERATOR+"VALID").encode('utf-8', 'replace'))
			elif packet.type == PACKET.PACKAGE_RECIEVE.value:
				print("PACKAGE_RECIEVE")
				package = getPackage(cursor).fetchone();
				conn.send(("2"+SEPERATOR+str(package[0])+SEPERATOR+str(package[1])+SEPERATOR+str(package[2])+SEPERATOR+str(package[3])+SEPERATOR+str(package[4])+SEPERATOR+str(package[5])).encode('utf-8', 'replace'))
				print("Sent back package")
			elif packet.type == PACKET.REQUEST_LEADERBOARD.value:
				print("LEADERBOARD_REQUEST")
				package = getRankChunk(cursor, packet.chunksize, packet.offset).fetchall();
				print("PACKAGE INFO: ")
				print(package)
				#encode info
				outdata = str(PACKET.REQUEST_LEADERBOARD.value)
				for x in package:
					outdata += SEPERATOR + decodeLeaderBoardPacket(x)
				#send info back
				conn.send(outdata.encode('utf-8', 'replace'))
			elif packet.type == PACKET.REQUEST_USERRANK.value:
				print("REQUEST_USERRANK\n data:")
				userRankData = getUserRank(cursor, packet.nameOfUser).fetchone()
				print(userRankData)
				#decode
				outdata = str(PACKET.REQUEST_USERRANK.value)
				for x in userRankData:
					outdata += SEPERATOR + str(x)
				#send info back
				conn.send(outdata.encode('utf-8', 'replace'))

			else:
				print("Unknown Package Type " + data[0])
				conn.send(("0"+SEPERATOR+"UNKNOWN").encode('utf-8', 'replace'))
		else:
			print("Bad Packet Type")
			conn.send(("0"+SEPERATOR+"INVALID").encode('utf-8', 'replace'))
	except:
		print("There was a problem with the request")
	#disconnect
	print("Disconnecting From Database...")
	cursor.reset()
	cursor.close()
	db.close()
	print("Terminating Connection...")
	conn.close()


#Create Local Server
print("Creating Local Socket...")
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM) #Creates socket
s.setsockopt(socket.SOL_SOCKET,socket.SO_REUSEADDR,1)
s.bind((HOST, PORT))
s.listen(15) #accepts connections backlog

#read login from file
credFileLoc = "creds.txt"
credFile = open(credFileLoc, 'r')
dbUsername = credFile.readline().rstrip("\n")
dbPassword = credFile.readline().rstrip("\n")
dbTable = credFile.readline().rstrip("\n")
credFile.close()

print("Testing Database Connection...")

#connect to db
db_T = mysql.connector.connect(host="localhost",user=dbUsername, password=dbPassword, database=dbTable, charset='utf8', use_unicode=True)

print("Connected To Database Successfully!")

#create cursor
cursor_T = db_T.cursor()

print("Ready To Query Database!")

#close db connection test
cursor_T.close()
db_T.close()

print("Starting Local Server...")
while 1:
	conn, addr = s.accept()
	print('Client connected ' + addr[0] + ':' + str(addr[1]))
	start_new_thread(clientThread, (conn,))

s.close()

print("Done.")
