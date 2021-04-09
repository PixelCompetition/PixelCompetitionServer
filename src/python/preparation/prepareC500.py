import numpy as np
import random
from Preparer import Preparer
from Riddle import Riddle


class PreparerC500(Preparer):
    Limit = 0
    Primes = None

    def __init__(self, limit):
        super().__init__("C500", 2 ** 26)
        self.Limit = limit
        print("Searching for primes smaller than " + str(self.Limit))
        self.Primes = self.primesfrom2to(self.Limit)
        print("Found " + str(len(self.Primes)) + " primes")

    @staticmethod
    def primesfrom2to(n):
        """ Input n>=6, Returns a array of primes, 2 <= p < n """
        sieve = np.ones(n // 3 + (n % 6 == 2), dtype=np.bool)
        for i in range(1, int(n ** 0.5) // 3 + 1):
            if sieve[i]:
                k = 3 * i + 1 | 1
                sieve[k * k // 3::2 * k] = False
                sieve[k * (k - 2 * (i & 1) + 4) // 3::2 * k] = False
        return np.r_[2, 3, ((3 * np.nonzero(sieve)[0][1:] + 1) | 1)]

    def createRiddle(self, counter):
        count = np.random.randint(4, 8)
        prod = 1
        riddle = ""
        for num in random.choices(self.Primes, k=count):
            riddle = riddle + " " + str(num)
            p = int(num)
            prod = prod * p
        return Riddle(counter, riddle, riddle, str(prod))


if __name__ == "__main__":
    print("Start")
    lim = 256 * 256 * 256 * 256 - 1
    prep = PreparerC500(lim)
    prep.execute()
    print("Done")
