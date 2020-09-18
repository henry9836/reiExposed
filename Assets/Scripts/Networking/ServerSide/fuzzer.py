import socket
import random

IP = "45.32.245.198"
FILE = "fuzz.txt"
PORT = 27100
MAXRECV = 2048
SEPERATOR = "--"


def sendServer(data):
	currP = "10"
	itemP1 = "1"
	itemP2 = "2"
	itemP3 = "3"
	name = "FUZZERMAN"
	time = str(random.randint(1, 4000))
	if randomMode:
		currP = str(random.randint(-9999, 9999))
		itemP1 = str(random.randint(-9999, 9999))
		itemP2 = str(random.randint(-9999, 9999))
		itemP3 = str(random.randint(-9999, 9999))
	packet = "1" + SEPERATOR +"FUZZER" + SEPERATOR + data + SEPERATOR + currP + SEPERATOR + itemP1 + SEPERATOR + itemP2 + SEPERATOR + itemP3 + SEPERATOR + name + SEPERATOR + time
	sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
	sock.connect(server_address)
	if user_in == "y":
		print("Sending Packet: [" + packet + "]")
	sock.send(packet.encode('utf-8', 'replace'))
	data = sock.recv(MAXRECV)
	print(data.decode('utf-8', 'replace'))
	sock.close()

sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_address = (IP, PORT)
user_in = input("Would you like to fuzz manually [N/y]")
user_randomMode = input("Would you like to fuzz game values? [N/y]")
randomMode = False;

if user_randomMode == "y":
	randomMode = True

if user_in == "y":
	sendServer(input("Message: "))
else:
	#get amount of lines
	lineCount = sum(1 for line in open(FILE, encoding='utf-8'))
	print("Fuzzing Server With File: " + FILE)
	count = 1
	with open(FILE, "r", encoding='utf-8') as f:
		lines = f.readlines()

	for x in lines:
		print(str(round((count/lineCount)*100, 1)) + "% Complete")
		sendServer(x)
		count += 1
			


print("Done.")
