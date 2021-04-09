import os
import glob
from PIL import Image, ImageDraw, ImageFont
exec(open("../generalVars.py").read())


def drawFont(ttf, text, target):
    print("Found font:" + ttf)
    img = Image.new('RGB', (500, 60), color=(0, 0, 0))
    font = ImageFont.truetype(ttf, 50)
    sample = os.path.basename(ttf) + "\n" + text
    draw_txt = ImageDraw.Draw(img)
    width, height = draw_txt.textsize(sample, font=font)
    img = Image.new('RGB', (width + 12, height + 30), color=(0, 0, 0))
    draw_txt = ImageDraw.Draw(img)
    draw_txt.text((6, 6), sample, font=font, fill=(255, 255, 255))
    savename = os.path.join(target, os.path.basename(ttf) + ".png")
    img.save(savename)


def findFonts(text):
    cfg = Config()
    target = cfg.getFontsFolder()
    pattern = os.path.join(cfg.SystemFontFolder, "*.ttf")
    for ttf in glob.glob(pattern):
        drawFont(ttf, text, target)


if __name__ == "__main__":
    print("Start")
    findFonts("groß und KLeiN")
    print("Done")



