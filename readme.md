# Chirp
## Authors
- Daniel Sølvsten Millard - dmil@itu.dk 
- Frederik Lund Rosenlund - frlr@itu.dk
- Jacob Pærregaard - jacp@itu.dk
- Mads Nørklit Jensen - macj@itu.dk 
- Rasmus Nielsen - raln@itu.dk

## Running the program:
The program can be run using `dotnet run`.
#### Valid commands
- Write a cheep: `dotnet run cheep "<cheep here>"`
- Read a number of cheeps (`default: 10`): `dotnet run read <number>` 

## How to make commits with co-Authors
- Write `git commit`, you will then enter vim or similar
- First line contains title of commit (should be in imperative and no longer than 50 characters)
- Next line should be left blank
- Next lines contain the description of the commit (each line should be no longer than 72 characters)
- Co-authors should be added to the bottom lines, each co-author on a seperate line in the following format:
  - Co-authored-by: Mads <macj@itu.dk>
  - Co-authored-by: Jacob <jacp@itu.dk>
  - Co-authored-by: Frederik <frlr@itu.dk>
  - Co-authored-by: Daniel <dmil@itu.dk>
  - Co-authored-by: Rasmus <raln@itu.dk>
 
## How to write issues (format)
- **Title:** As a `<type of user>`, i want `<a goal>` so that `<benefit>`
- **Body:**
  - **User story**
  - **Acceptance Criteria**
  - **Definition of Done**

### Example
As a user, I can log in through a third-party account.

Acceptance criteria:
  * Can log in through Microsoft
  * Can log in through Google
  * Can log in through GitHub

## How to make a release using tags
On your own PC you do `git tag` to see all current tags.
You then determine how large your update is, so we follow the proper version schematic.
You then create a new tag using `git tag <vx.x.x>` where x replaces the numbers.
When the tag is created, you push the tag using `git push origin <tag>`

