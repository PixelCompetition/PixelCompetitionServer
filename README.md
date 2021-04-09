# PixelCompetitionServer

PixelCompetition is an interactive programming competition that can run online during events or offline. We are using gitHUB to develop the server and provide public sample clients, for more information please refere to: https://pixel-competition.de

A PixelCompetition is a social experiment, all participants simultaneously draw pictures on a common canvas. There are different protocols over which a pixel can be drawn. During a competition new protocols are released, so participants can then race to create their own clients.

Since the implementation of new clients for new protocols is part of the competition, we face a problem with open-source. Not in the sense of licenses, but in the sense of open source is publicly available.

The problem: If we push the server code, the test clients and the preparation scripts for new competitions into a public repo, then the competitions are no longer *new* during a running game. Therefore, we have a script that cleans up our internal repo and then pushes the result into the public one while transfering all commit messages. Not super nice, but at least an approach.

**We invite everybody to implement own competitions, it would be great if you join in and implement your own ideas.** Unfortunately, the approach shown above makes the way back a bit cumbersome. If you implement a new competition the reasons shown above shows it would be a bad idea to publish a patch here. Please send us a patch to pixel@schlingensiepen.com and let us know when and how it should be published. (Any suggestions on how to improve this process would be appreciated.)
