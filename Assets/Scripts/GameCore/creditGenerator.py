from docx import *

class textObject:
    def __init__(self, line, style):
        self.line = line
        self.style = style

doc = Document("Credits.docx")
lines = []
encodedCredits = []

print("Reading Document...")

for i in doc.paragraphs:
    lines.append(textObject(i.text, i.style))

print("Done.")
print("Found " + str(len(lines)) + " lines.")

print("Converting Document...")

for i in lines:
    if (i.style.name == "normal"):
        if '\n' in i.line:
            print("Bad Line. " + i.line)
        encodedCredits.append("N#" + i.line + "--")
    elif (i.style.name == "Title"):
        if '\n' in i.line:
            print("Bad Line. " + i.line)
        encodedCredits.append("T#" + i.line + "--")
    elif (i.style.name == "Heading 1"):
        if '\n' in i.line:
            print("Bad Line. " + i.line)
        encodedCredits.append("T#" + i.line + "--")
    elif (i.style.name == "Heading 2"):
        if '\n' in i.line:
            print("Bad Line. " + i.line)
        encodedCredits.append("H#" + i.line + "--")
    else:
        print("Unknown Style " + i.style.name)

print("Writing Output To File...")

encodedString = ""

f = open('output.txt', 'w')

for i in encodedCredits:
    f.write(i)
f.close()
print("Done.")
