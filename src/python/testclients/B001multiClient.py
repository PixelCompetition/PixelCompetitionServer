import random
import socket
import numpy as np
from PIL import Image

exec(open("../generalVars.py").read())
Cfg = Config()

#HOST='box.pixel-competition.de'
HOST='localhost'

PORT=2342


imagePath = Cfg.getLogoPath()
width=1920
height=1080
socketCount=12

img = Image.open(imagePath)
img = img.convert('RGBA')
arr = np.array(img)

image_width = img.size[0]
image_height = img.size[1]

xOffset = random.randint(0, width - image_width)
yOffset = random.randint(0, height - image_height)

cmds = []
for x in range(0, image_width):
    for y in range(0, image_height):
        if (arr[y][x][3] == 255):
            r = arr[y][x][0]
            g = arr[y][x][1]
            b = arr[y][x][2]
            # PX 20 30 2 3 4
            # PX {x} {y} {r} {g} {b} Zeilenumbruch
            cmd = 'PX %d %d %d %d %d\n' % (x + xOffset, y + yOffset, r, g, b)
            cmds.append(cmd)
random.shuffle(cmds)

current = 0
def next():
    global current
    current = current + 1
    if current >= len(cmds):
        current = 0
    return cmds[current]

sockets = []
for i in range(0, socketCount):
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.connect((HOST,PORT))
    sockets.append(sock)

num = 0
counter = 0
while True:
    counter = counter + 1
    if (counter > 10000):
        counter = 0
        num = num + 1
        print (num)
    for sock in sockets:
        cmd = next()
        sock.send(cmd.encode())

print("ENDE")

