import os
import codecs
from Riddle import Riddle
import jsonpickle

exec(open("../generalVars.py").read())


class Preparer:
    Cfg = Config()
    CompetitionName = None
    DataPath = Cfg.DataFolder
    UrlPrefix = Cfg.UrlPrefix
    FolderDepth = 5
    RiddleCount = 1000000
    Riddles = None
    RiddlePerFile = 2 ** 14
    ReportChunkSize = 2 ** 12
    PostProcessors = []

    def __init__(self, competitionName, riddleCount):
        self.CompetitionName = competitionName
        self.RiddleCount = riddleCount

    def loadFile(self, fileName):
        fileName = os.path.join(self.Cfg.DataFolder, fileName)
        if not os.path.exists(fileName):
            return []
        with codecs.open(fileName, "r", encoding='utf-8') as infile:
            lines = []
            for line in infile.readlines():
                line = line.strip()
                lines.append(line)
        return lines

    def saveFile(self, lines, fileName):
        fileName = os.path.join(self.Cfg.DataFolder, fileName)
        with codecs.open(fileName, "w", encoding='utf-8') as outfile:
            for line in lines:
                line = line.strip()
                outfile.write(line)
                outfile.write("\n")


    def loadRiddles(self):
        print("Load Riddles for", self.CompetitionName)
        counter = 0
        self.Riddles = []
        for source, question, answer in zip(
                self.loadFile(self.CompetitionName + ".sources.txt"),
                self.loadFile(self.CompetitionName + ".questions.txt"),
                self.loadFile(self.CompetitionName + ".answers.txt")):
            self.Riddles.append(Riddle(counter, source, question, answer))
            counter += 1
        print(len(self.Riddles), "of", self.RiddleCount, "riddles loaded.")

    def saveRiddles(self):
        print("Save Riddles", len(self.Riddles), "of", self.RiddleCount)
        print("Save Sources")
        self.saveFile(map(lambda x: x.Source, self.Riddles), self.CompetitionName + ".sources.txt")
        print("Save Questions")
        self.saveFile(map(lambda x: x.Question, self.Riddles), self.CompetitionName + ".questions.txt")
        print("Save Answers")
        self.saveFile(map(lambda x: x.Answer, self.Riddles), self.CompetitionName + ".answers.txt")
        print("Riddles saved")

    def addRiddle(self, source, question, answer):
        count = len(self.Riddles)
        res = Riddle(count, source, question, answer)
        self.Riddles.append(res)
        return res

    def createRiddle(self, counter):
        raise NotImplementedError

    def createCompetition(self):
        self.loadRiddles()
        while len(self.Riddles) < self.RiddleCount:
            count = len(self.Riddles)
            if count % self.ReportChunkSize == 0:
                print("Created", count, "of", self.RiddleCount)
            riddle = self.createRiddle(count)
            riddle.Counter = count  # Safety first!
            self.Riddles.append(riddle)
        self.saveRiddles()

    def createFolder(self, path):
        if os.path.exists(path):
            return
        if len(path.strip()) == 0:
            return
        print(path)
        parent = os.path.dirname(path)
        self.createFolder(parent)
        os.mkdir(path)

    def generateUrl(self, counter):
        pattern = "{:0" + str(len(str(self.RiddleCount))) + "x}.png"
        name = pattern.format(counter)
        chunks = []
        for i in range(0, self.FolderDepth):
            chunks.append(name[i])
        url = self.UrlPrefix + self.CompetitionName + "/" + "/".join(chunks) + "/" + name
        return url

    def generateFileName(self, counter):
        pattern = "{:0" + str(len(str(self.RiddleCount))) + "x}.png"
        name = pattern.format(counter)
        chunks = [
            self.Cfg.getResultFolder("webserver", self.CompetitionName)
        ]
        for i in range(0, self.FolderDepth):
            chunks.append(str(name[i]))
        chunks.append(name)
        fullPath = os.path.join(*chunks)
        self.createFolder(os.path.dirname(fullPath))
        return fullPath

    LoadCount = -1

    def loadRiddles_(self):
        print("Load Riddles for", self.CompetitionName)
        fileName = self.Cfg.getDataFileName(self.CompetitionName) + ".json"
        if not os.path.exists(fileName):
            self.Riddles = []
        else:
            try:
                with codecs.open(fileName, "r", "utf-8") as fp:
                    self.Riddles = jsonpickle.loads(fp.read())
            except IOError:
                self.Riddles = []
        self.LoadCount = len(self.Riddles)
        print(self.LoadCount, "loaded.")

    def saveRiddles_(self):
        if len(self.Riddles) != self.LoadCount:
            return
        print("Save Riddles for", self.CompetitionName)
        with codecs.open(self.Cfg.getDataFileName(self.CompetitionName) + ".json", "w", "utf-8") as fp:
            fp.write(jsonpickle.dumps(self.Riddles))
        print(len(self.Riddles), "saved.")

    def storeCompetition(self):
        print("Store", len(self.Riddles), "Riddles for ", self.CompetitionName)
        target = self.Cfg.getResultFolder("pixelserver", self.CompetitionName)
        self.createFolder(target)
        # answer
        # question
        counter = 0
        fileCounter = 0
        file = None
        for riddle in self.Riddles:
            if counter % self.RiddlePerFile == 0:
                if file:
                    file.close()
                fileName = os.path.join(target, "Riddle" + str(fileCounter).zfill(8) + ".txt")
                print(fileName)
                fileCounter += 1
                file = codecs.open(fileName, "w", "utf-8")
            file.writelines([riddle.Answer, "\n", riddle.Question, "\n"])
            counter += 1
        if file:
            file.close()
        print("Stored.")

    def postProcessCompetition(self):
        for postProcessor in self.PostProcessors:
            counter = 0
            for riddle in self.Riddles:
                counter += 1
                postProcessor.postProcess(riddle)
                if counter % self.ReportChunkSize == 0:
                    print("Post process " + str(counter) + " of " + str(self.RiddleCount))

    def execute(self):
        print("Create Riddles")
        self.createCompetition()
        print("Store Riddles")
        self.storeCompetition()
        print("Post Processing")
        self.postProcessCompetition()
        print("Done")
