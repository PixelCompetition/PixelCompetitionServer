from PreparerWords import PreparerWords
from PostProcessorImage import PostProcessorImage


class PreparerC600(PreparerWords):

    def __init__(self):
        super().__init__("C600", 2**20, 10000,
                         ["pixelcompetition", "PixelCompetition", "mag.lab", "C3", "Chaos",
                          "Magrathea", "OLaF", "CCC", "Cyber"])
        self.PostProcessors = [PostProcessorImage(self, self.Cfg)]


if __name__ == "__main__":
    print("Start")
    prep = PreparerC600()
    prep.execute()
    print("Done")

