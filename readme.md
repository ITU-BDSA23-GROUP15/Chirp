# Chirp
## Authors
- Daniel Sølvsten Millard - dmil@itu.dk 
- Frederik Lund Rosenlund - frlr@itu.dk
- Jacob Pærregaard - jacp@itu.dk
- Mads Nørklit Jensen - macj@itu.dk 
- Rasmus Nielsen - raln@itu.dk

## How to make commits with co-Authors
- Write git commit, you will then enter vim or similar
- First line contains title of commit (should be in imperative and no longer than 50 characters)
- Next line should be left blank
- Next lines contain the description of the commit (each line should be no longer than 72 characters)
- Co-authors should be added to the bottom lines, each co-author on a seperate line in the following format:
- Co-authored-by: Mads <macj@itu.dk>
- Co-authored-by: Jacob <jacp@itu.dk>

## Running the program:
The program can be run using `dotnet run`.
#### Valid commands
- Write a cheep: `dotnet run cheep "<cheep here>"`
- Read all cheeps: `dotnet run read`
- Read a number of cheeps: `dotnet run read <number>`
