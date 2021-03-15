import os
from PostProcessor import PostProcessor
from FontProvider import FontProvider
from captcha.image import ImageCaptcha


class PostProcessorCaptcha(PostProcessor):
    Fonts = None

    def __init__(self, preparer, config):
        super().__init__(preparer, config)
        self.Fonts = FontProvider(config)

    def postProcess(self, riddle):
        path = self.Preparer.generateFileName(riddle.Counter)
        if os.path.exists(path):
            return
        print("Generate Captcha", path)
        image = ImageCaptcha()
        image._truefonts = self.Fonts.getFonts()
        image.write(riddle.Source, path)
