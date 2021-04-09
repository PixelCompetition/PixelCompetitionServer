from io import BytesIO
import os
import random
import glob
from PIL import ImageFont
exec(open("../generalVars.py").read())


class FontProvider:
    TTFs = None
    Cfg = None

    def __init__(self, config):
        self.Cfg = config

    def loadFonts(self):
        if self.TTFs is not None:
            return self.TTFs

        validFolder = self.Cfg.getFontsFolder()
        pattern = os.path.join(self.Cfg.SystemFontFolder, "*.ttf")
        ttfs = []
        for ttf in glob.glob(pattern):
            validFile = os.path.join(validFolder, os.path.basename(ttf) + ".png")
            if not os.path.exists(validFile):
                continue
            file = open(ttf, "rb")
            bytes_font = BytesIO(file.read())
            ttfs.append(ImageFont.truetype(bytes_font, 35))
        self.TTFs = ttfs
        return self.TTFs

    def getFont(self):
        ttfs = self.loadFonts()
        return random.choice(ttfs)

    def getFonts(self):
        ttfs = self.loadFonts()
        return ttfs


if __name__ == "__main__":
    print("Start")
    prov = FontProvider(Config())
    for font in prov.getFonts():
        print(font.getname())
    print("Done")

