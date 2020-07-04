import socket

IP = "45.32.245.198"
FILE = "fuzz.txt"
PORT = 27010
MAXRECV = 2048


def sendServer(data):
	packet = "1--FUZZER--" + data + "--12--1--1--0"
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
