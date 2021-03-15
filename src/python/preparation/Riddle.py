class Riddle:
    Counter = 0
    Source = ""
    Question = ""
    Answer = ""

    def __init__(self, counter, source, question, answer):
        self.Counter = counter
        self.Source = source.strip()
        self.Question = question.strip()
        self.Answer = answer.strip()


