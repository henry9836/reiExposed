import mysql.connector

#CREATE A ENTRY
def createPackage(ID, MSG, CURR, A1, A2, A3):
	q = "INSERT INTO Packages (ID, MSG, CURR, ATTACH1, ATTACH2, ATTACH3) VALUES (%s, %s, %s, %s, %s, %s)"
	v = (ID, MSG, CURR, A1, A2, A3);
	cursor.execute(q, v)
	#save changes
	db.commit()
	print("Code here :)")

def getPackage():
	q = "SELECT * FROM Packages ORDER BY RAND() LIMIT 1"
	cursor.execute(q)
	for x in cursor:
		print(x)
	print("Code here :)")

#init
dbUsername = ""
dbPassword = ""
dbTable = ""

#read login from file
credFileLoc = "creds.txt"
credFile = open(credFileLoc, 'r')
dbUsername = credFile.readline().rstrip("\n")
dbPassword = credFile.readline().rstrip("\n")
dbTable = credFile.readline().rstrip("\n")
credFile.close

print("Connecting With Creds:")
print(dbUsername)
print(dbPassword)
print(dbTable)

#connect to db
db = mysql.connector.connect(host="localhost",user=dbUsername, password=dbPassword, database=dbTable)

print("Connected To Database!")

#
cursor = db.cursor()
cursor.execute("SHOW TABLES")
for x in cursor:
	print(x)

createPackage("1234Asdf", "Python Is Cool!", 1234, 1, 1, 0)
print("Getting Random Package...")
getPackage()
print("Done.")
