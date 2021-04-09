class PostProcessor:
    Preparer = None
    Config = None

    def __init__(self, preparer, config):
        self.Preparer = preparer
        self.Config = config

    def postProcess(self, riddle):
        pass

