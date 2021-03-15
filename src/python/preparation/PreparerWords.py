import codecs
import random

from Preparer import Preparer


class PreparerWords(Preparer):

    GWords = None
    AdditionalWords = []
    AdditionalWordsCount = 0

    def __init__(self, competitionName, riddleCount, additionalWordsCount, additionalWords):
        super().__init__(competitionName, riddleCount)
        self.AdditionalWords = additionalWords
        self.AdditionalWordsCount = additionalWordsCount

    def createRiddle(self, counter):
        pass

    def createWordRiddle(self, counter, source):
        return self.generateUrl(counter), source  # in Word Competitions source and answer is the same

    def createCompetition(self):
        self.loadRiddles()
        if len(self.Riddles) >= self.RiddleCount:
            return

        with codecs.open(self.Cfg.getWordsFileName(), "r", "utf-8") as f:
            self.GWords = list([line.rstrip() for line in f])

        wordsToAdd = []

        if len(self.Riddles) == 0:  # very firstRun
            for i in range(0, self.AdditionalWordsCount):
                wordsToAdd += self.AdditionalWords

        while len(self.Riddles) + len(wordsToAdd) < self.RiddleCount:
            wordsToAdd += self.GWords

        random.shuffle(wordsToAdd)
        for wordToAdd in wordsToAdd:
            counter = len(self.Riddles)
            riddle, answer = self.createWordRiddle(counter, wordToAdd)
            self.addRiddle(wordToAdd, riddle, answer)
            if len(self.Riddles) >= self.RiddleCount:
                break
        self.saveRiddles()
