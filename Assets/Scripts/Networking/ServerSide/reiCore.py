from _thread import *
import mysql.connector
import socket
import enum

#init
dbUsername = ""
dbPassword = ""
dbTable = ""
HOST = '' #All interfaces
PORT = 27010
MAXRECV = 2048
SEPERATOR = "--"
ERROR_GENERAL = -1

#CONNECTION INFO
class PACKET(enum.Enum): 
    ACK = 0
    PACKAGE_SEND = 1
    PACKAGE_RECIEVE = 2

class packetStruct:
	def __init__(self, input):
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
			try:
				self.ID = self.data[1]
				self.msg = self.data[2]
				self.curr = int(self.data[3])
				self.item1 = int(self.data[4])
				self.item2 = int(self.data[5])
				self.item3 = int(self.data[6])
			except:
				print("Invalid Packet Structure")
				self.type = ERROR_GENERAL #error value
				return;
		#RECIEVE
		#Nothing Values
		elif self.type == PACKET.ACK.value or  self.type == PACKET.PACKAGE_RECIEVE.value:
			pass
		else:
			print("Unknown Packet Type {" + str(self.type) + "}")
			print(PACKET.ACK.value)
			print(type(PACKET.ACK.value))
			self.type = ERROR_GENERAL #error value
			return;

#CREATE A ENTRY
def createPackage(packet, _cursor, _db):
	q = "INSERT INTO Packages (ID, MSG, CURR, ATTACH1, ATTACH2, ATTACH3) VALUES (%s, %s, %s, %s, %s, %s)"
	v = (packet.ID, packet.msg, packet.curr, packet.item1, packet.item2, packet.item3);
	_cursor.execute(q, v)
	#save changes
	_db.commit()
	print("Code here :)")

#GET A RANDOM ENTRY
def getPackage(_cursor):
	q = "SELECT * FROM Packages ORDER BY RAND() LIMIT 1"
	_cursor.execute(q)
	return _cursor

def clientThread(conn):
	#Connect To DB
	print("Thread Connecting To Database...")
	db = mysql.connector.connect(host="localhost",user=dbUsername, password=dbPassword, database=dbTable)

	print("Connected To Database!")

	#create cursor
	cursor = db.cursor()

	data = conn.recv(MAXRECV)
	data = data.decode().rstrip("\n");
	print(data)

	#Decode Packet
	packet = packetStruct(data)

	#Is Valid Packet?
	if packet.type >= 0:
		if packet.type == PACKET.ACK.value:
			print("ACK")
			conn.send(("0"+SEPERATOR+"ACK").encode())
		elif packet.type == PACKET.PACKAGE_SEND.value:
			print("PACKAGE_SEND")
			createPackage(packet, cursor, db);
			conn.send(("0"+SEPERATOR+"VALID").encode())
		elif packet.type == PACKET.PACKAGE_RECIEVE.value:
			print("PACKAGE_RECIEVE")
			package = getPackage(cursor).fetchone();
			conn.send(("2"+SEPERATOR+str(package[1])+SEPERATOR+str(package[2])+SEPERATOR+str(package[3])+SEPERATOR+str(package[4])+SEPERATOR+str(package[5])).encode())
		else:
			print("Unknown Package Type " + data[0])
			conn.send(("0"+SEPERATOR+"UNKNOWN").encode())
	else:
		conn.send(("0"+SEPERATOR+"INVALID").encode())

	#disconnect
	print("Disconnecting From Database...")
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
db_T = mysql.connector.connect(host="localhost",user=dbUsername, password=dbPassword, database=dbTable)

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
