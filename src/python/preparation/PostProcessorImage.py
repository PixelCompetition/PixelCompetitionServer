import os
from PostProcessor import PostProcessor
from FontProvider import FontProvider
from PIL import Image
from PIL import ImageDraw


class PostProcessorImage(PostProcessor):

    Fonts = None

    def __init__(self, preparer, config):
        super().__init__(preparer, config)
        self.Fonts = FontProvider(config)

    def postProcess(self, riddle):
        path = self.Preparer.generateFileName(riddle.Counter)
        if os.path.exists(path):
            return
        print("Generate Image", path)
        img = Image.new('RGB', (100, 30), color=(255, 255, 255))
        font = self.Fonts.getFont()
        sample = riddle.Source
        draw_txt = ImageDraw.Draw(img)
        width, height = draw_txt.textsize(sample, font=font)
        img = Image.new('RGB', (width + 8, height + 12), color=(0, 0, 0))
        draw_txt = ImageDraw.Draw(img)
        draw_txt.text((4, 4), sample, font=font, fill=(255, 255, 255))
        img.save(path)
