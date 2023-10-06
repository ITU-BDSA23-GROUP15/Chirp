# Chirp
Application is deployed here: https://bdsagroup15chirprazor.azurewebsites.net/

## Authors
- Daniel Sølvsten Millard - dmil@itu.dk 
- Frederik Lund Rosenlund - frlr@itu.dk
- Jacob Pærregaard - jacp@itu.dk
- Mads Nørklit Jensen - macj@itu.dk 
- Rasmus Nielsen - raln@itu.dk

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

## Releases

### How to make a release using tags
On your own PC you do `git tag` to see all current tags.
You then determine how large your update is, so we follow the proper version schematic.
You then create a new tag using `git tag <vx.x.x>` where x replaces the numbers.
When the tag is created, you push the tag using `git push origin <tag>`

### Semantic Versioning
Given a version number MAJOR.MINOR.PATCH, increment the:

* MAJOR version when you make incompatible API changes
* MINOR version when you add functionality in a backward compatible manner
* PATCH version when you make backward compatible bug fixes
* Additional labels for pre-release and build metadata are available as extensions to the MAJOR.MINOR.PATCH format.
* See https://semver.org for further documentation on semantic versioning

## Processes we utilize

### Pair programming
* Proper pair programming: Sit in groups of 2-3 people, one person is a driver (the person actually coding) whilst the rest are navigators, giving suggestions to the driver. Do this for 15-20 minutes, then switch the driver. Before switching, the person who was the driver, commits what has currently been done.

### Code review
* Pull requests. Whenever anything is ready to be merged into the main branch, make a pull request, and put some
or all members of the organization, who has not contributed to this code, as reviewers.
