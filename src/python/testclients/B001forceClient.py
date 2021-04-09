import random
import socket
import itertools

#HOST = 'box.pixel-competition.de'
HOST = "localhost"

PORT = 2342
width = 1920
height = 1080
socketCount = 128


xx = list(range(0, width))
yy = list(range(0, height))
allPixels = list(itertools.product(xx, yy))


def stripes():
    print("Stripes")
    stripe_width = 64
    x=0
    y=0
    # connect socket
    sockets = []
    for i in range (0, socketCount):
      sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
      sock.connect((HOST, PORT))
      sockets.append(sock)
    for i in range (0, int(width / stripe_width)):
        r, g, b = random.randint(0,255), random.randint(0,255), random.randint(0,255)
        print ("Color", r, g, b)
        for i in range (0, int (stripe_width*height/len(sockets))):
            for s in sockets:
                x = x + 1
                if x >= height:
                    y = y + 1
                    x = 0
                    if y >= width:
                        y = 0
                s.send(('PX %d %d %d %d %d\n' % (y,x,r,g,b)).encode())
    for s in sockets:
        s.close()

def pixels():
    print("Pixels")
    sockets=[]
    for i in range (0,socketCount):
      sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
      sock.connect((HOST, PORT))
      sockets.append(sock)

    random.shuffle(allPixels)
    for x,y in allPixels:
        r, g, b = random.randint(0,255), random.randint(0,255), random.randint(0,255)
        s = random.choice(sockets)
        s.send(('PX %d %d %d %d %d\n' % (x,y,r,g,b)).encode())
    for s in sockets:
        s.close()

while True:
    stripes()
    pixels()
