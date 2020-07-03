from _thread import *
import mysql.connector
import socket
import enum

#init
dbUsername = ""
dbPassword = ""
dbTable = ""
HOST = '' #All interfaces
PORT = 27110
MAXRECV = 2048
SEPERATOR = "--"

#CONNECTION INFO
class PACKET(enum.Enum): 
    ACK = 0
    PACKAGE_SEND = 1
    PACKAGE_RECIEVE = 2

class packetStruct:
	def __init__(self, input):
		self.data = self.extractData(input)


	def extractData(input):
		return input.split(SEPERATOR)

#CREATE A ENTRY
def createPackage(ID, MSG, CURR, A1, A2, A3):
	q = "INSERT INTO Packages (ID, MSG, CURR, ATTACH1, ATTACH2, ATTACH3) VALUES (%s, %s, %s, %s, %s, %s)"
	v = (ID, MSG, CURR, A1, A2, A3);
	cursor.execute(q, v)
	#save changes
	db.commit()
	print("Code here :)")

def getPackage(_cursor):
	q = "SELECT * FROM Packages ORDER BY RAND() LIMIT 1"
	_cursor.execute(q)
	return _cursor

def clientThread(conn):
	#Connect To DB
	print("Thread Connecting To DataBase...")
	db = mysql.connector.connect(host="localhost",user=dbUsername, password=dbPassword, database=dbTable)

	print("Connected To Database!")

	#create cursor
	cursor = db.cursor()

	data = conn.recv(MAXRECV)
	data = data.decode().rstrip("\n");
	print(data)
	if data[0] == "0":
		print("ACK")
		conn.send("0--".encode())
	elif data[0] == "1":
		print("PACKAGE_SEND")
		createPackage();
	elif data[0] == "2":
		print("PACKAGE_RECIEVE")
		getPackage(cursor);
	else:
		print("Unknown Package Type " + data[0])
	'''except:
		print("FATAL Error cannot process data, closing connection...")'''
	#outdata = "Processing Data From Client"
	#conn.send(outdata.encode())

	#disconnect
	cursor.close()
	db.close()
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
credFile.close

print("Testing Database Connection...")

#connect to db
db = mysql.connector.connect(host="localhost",user=dbUsername, password=dbPassword, database=dbTable)

print("Connected To Database Successfully!")

#create cursor
cursor = db.cursor()
print("Ready To Query Database!")

print("Starting Local Server...")
while 1:
	conn, addr = s.accept()
	print('Client connected ' + addr[0] + ':' + str(addr[1]))
	start_new_thread(clientThread, (conn,))

#createPackage("1234Asdf", "Python Is Cool!", 1234, 1, 1, 0)
#print("Getting Random Package...")
#getPackage()

s.close()

print("Done.")
