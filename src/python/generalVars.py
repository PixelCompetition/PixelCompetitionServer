import os

class Config:
    file = os.path.realpath(__file__)
    ProjectFolder = os.path.dirname(os.path.dirname(os.path.dirname(os.path.dirname(file))))
    RootFolder = os.path.dirname(ProjectFolder)
    DataFolder = os.path.join(RootFolder, "pixelcompetition-data")  # TODO set you data folder here

    SourceFolder = os.path.join(ProjectFolder, "src")
    PythonFolder = os.path.join(SourceFolder, "python")
    ClientFolder = os.path.join(PythonFolder, "testclients")
    PreparationFolder = os.path.join(PythonFolder, "preparation")

    DotNetFolder = os.path.join(SourceFolder, "dotnet")

    DocFolder = os.path.join(ProjectFolder, "doc")
    WebFolder = os.path.join(ProjectFolder, "web")

    ImageFolder = os.path.join(DocFolder, "img")

    SystemFontFolder = "C:\\Windows\\fonts" #TODO setup your ttf-Font-Folder

    UrlPrefix = "https://www.pixelcompetition.de/download/" # TODO fill in your download folder

    def competitionFolder(self,name):
        return os.path.join(self.DataFolder, name)

    def reportFolder (self, name, folder):
        report = "found at" if os.path.exists(folder) else "not found at"
        print (name, report, folder)

    def getImages(self):
        return []

    def getDataFileName(self, name):
        return os.path.join(self.DataFolder, name)

    def getResultFolder(self, server, competition):
        return os.path.join(self.DataFolder, server, competition)

    def getWordsFileName(self):
        return self.getDataFileName("words.txt")

    def getFontsFolder(self):
        fontFolder = self.getDataFileName("allFonts")
        if not os.path.exists(fontFolder) : os.mkdir(fontFolder)
        return fontFolder

    def __init__(self):
        self.reportFolder("RootFolder", self.RootFolder)
        self.reportFolder("DataFolder", self.DataFolder)

        self.reportFolder("ProjectFolder", self.ProjectFolder)
        self.reportFolder ("DocFolder", self.DocFolder)
        self.reportFolder("ImageFolder", self.ImageFolder)

        self.reportFolder ("SourceFolder", self.SourceFolder)
        self.reportFolder ("DotNetFolder", self.DotNetFolder)
        self.reportFolder ("PythonFolder", self.PythonFolder)

        self.reportFolder ("ClientFolder", self.ClientFolder)

        self.reportFolder ("WebFolder", self.WebFolder)

        self.reportFolder("SystemFontFolder", self.SystemFontFolder)


if __name__ == "__main__":
    print("Start")
    cfg = Config()
    print("Images:")
    for image in cfg.getImages():
        print (image)
    print("Done")

Cfg = Config()