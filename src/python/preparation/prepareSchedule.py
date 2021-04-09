import csv
import codecs


def readCsv():
    header = None
    days = []
    rows = []

    with codecs.open("schedule.csv", "r", encoding='utf-8') as csvfile:
        reader = csv.reader(csvfile, delimiter='\t')
        for row in reader:
            for col in row:
                print(col)
            if header is None:
                header = row
            else:
                rows.append(row)

    for head in header:
        print(head)
        if head.strip() == "":
            continue
        days.append(head.strip())

    return header, days, rows


def writeXml(days, rows):
    contentcol = 3
    last = ""
    with codecs.open("schedule.xml", "w", encoding='utf-8') as outFile:
        for day in days:
            for row in rows:
                while len(row) < 2 * len(days) + 2:
                    row.append("")
                start = day + "T" + row[0] + ":00+01:00"
                content = row[contentcol]
                ctype = row[contentcol - 1]
                if content.strip() == "":
                    continue
                if ctype == "w":
                    content = "B001"
                if content != last:
                    outFile.write(
                        "<Schedule><Start>" + start + "</Start><Competition>" + content + "</Competition></Schedule>\n")
                    last = content
            contentcol = contentcol + 2


def shade(celltype, cellcontent):
    if cellcontent == "":
        cellcontent = " "
    if celltype == "w":
        return "bgcolor=#6bf|[[PixelCompetition/" + cellcontent.replace("#", "sharp") + "|" + cellcontent + "]]"
    if celltype == "s":
        return "bgcolor=#afa|[[PixelCompetition/" + cellcontent.replace("#", "sharp") + "|" + cellcontent + "]]"
    if celltype == "m":
        return "bgcolor=#ffa|[[PixelCompetition/" + cellcontent.replace("#", "sharp") + "|" + cellcontent + "]]"
    if celltype == "h":
        return "bgcolor=#f66|[[PixelCompetition/" + cellcontent.replace("#", "sharp") + "|" + cellcontent + "]]"
    return "bgcolor=#aaa|" + cellcontent


def writeWiki(days, rows):
    with codecs.open("schedule.txt", "w", encoding='utf-8') as outFile:
        outFile.write("{| class=\"wikitable\"" + '\r\n')
        outFile.write("|-" + '\r\n')
        outFile.write("!Beginn!!" + "!!".join(days) + '\r\n')
        outFile.write("|-" + '\r\n')
        for row in rows:
            print(row)
            while len(row) < 2 * len(days) + 2:
                row.append("")
            elements = [row[0]]
            for i in range(3, 2 * len(days) + 3, 2):
                elements.append(shade(row[i - 1], row[i]))
            outFile.write("|" + "||".join(elements) + '\r\n')
            outFile.write("|-" + '\r\n')
        outFile.write("|}" + '\r\n')


def prepareSchedule():
    header, days, rows = readCsv()
    writeXml(days, rows)
    writeWiki(days, rows)


if __name__ == "__main__":
    print("Start")
    prepareSchedule()
    print("Done")

