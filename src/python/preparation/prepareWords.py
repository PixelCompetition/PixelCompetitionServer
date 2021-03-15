import codecs
import os

exec(open("../generalVars.py").read())
cfg = Config()

# TODO insert additional word files for your languages
wordsInputFiles = [
    cfg.getDataFileName("deu-de_web-wrt_2019_100M-words.txt"),
    cfg.getDataFileName("eng_news_2016_1M-words.txt")
]
wordsOutputFile = cfg.getWordsFileName()

for wordsInputFile in wordsInputFiles:
    if not os.path.exists(wordsInputFile):
        print("Did not find Corpora, you may download it from https://wortschatz.uni-leipzig.de/en/download/German")
        exit()

ignorechars = ",;.:-´°^`#'*+~*/\"\t<>|ÄÖÜöäüß- ✓▼├╝│→›‹"
words = []


def ignore(text):
    UpperCounter = 0
    for char in text:
        if 64 < ord(char) < 91:
            UpperCounter = UpperCounter + 1
            continue
        if 96 < ord(char) < 123:
            continue
        return True
    return UpperCounter > 1


for wordsInputFile in wordsInputFiles:
    with codecs.open(wordsInputFile, "r", encoding='utf-8') as infile:
        print("Loading", wordsInputFile)
        for line in infile.readlines():
            chunks = line.split("\t")
            token = chunks[1].strip()
            if len(token) < 5:
                continue
            if ignore(token):
                continue
            number = int(chunks[2])
            if number < 25:
                break
            print("Adding", token)
            words.append(token)

print("Sorting", len(words), "words.")
words.sort()

print("Writing", len(words), "words to", wordsOutputFile)
with codecs.open(wordsOutputFile, "w", encoding='utf-8') as outfile:
    for word in words:
        outfile.write(word)
        outfile.write("\n")
        print(word)

print(len(words), "words converted from", ", ".join(wordsInputFiles), "to", wordsOutputFile)
print("Done")
