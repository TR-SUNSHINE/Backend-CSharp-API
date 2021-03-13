# Backend-CSharp-API

As part of this project the C# backend API repository will be used to version control the source code for the backend API's developed in C# .NET Core using the AWS serverless application framework. 
<br>
<br>
A serverless architecture is a way to build and run applications and services without having to manage infrastructure.
<br>
## Steps to clone the repository locally
### Navigate to working directory <br>
git clone https://github.com/TR-SUNSHINE/Backend-CSharp-API <br>
### Install additional frameworks and components <br/>
-AWS Serverless application framework <br/>
-AWS Toolkit<br/>
### Deploy Project <br/>
-cd "LambdaCSharpWebAPI/src/LambdaCSharpWebAPI" <br/>
-dotnet lambda deploy-serverless <br/>
### Test Project <br/>
-cd "LambdaCSharpWebAPI/test/LambdaCSharpWebAPI.Tests" <br/>
-dotnet test <br/>

## Repository Rules

### Change Type
Throughout the project there will be changes that fall into different categories depending on the piece of work that is going to be delivered. Each change type is categorised as below:

FEAT: The new feature you're adding to a particular application <br/>
FIX: A bug fix <br/>
STYLE: Feature and updates related to styling <br/>
REFACTOR: Refactoring a specific section of the codebase <br/>
TEST: Everything related to testing <br/>
DOCS: Everything related to documentation

### New Branches
A branch represents an independent line of development. Branches serve as an abstraction for the edit/stage/commit process. You can think of them as a way to request a brand new working directory, staging area, and project history.

The below naming convention and rules should be used when creating a new branch:

1) The new branch name must be created with the change type (listed above) in capital letters, followed by underscore, followed by a short description of what you are developing all in lowercase
2) If the feature name is more than one word then each word must be separated with a _

#### Usage
1) For creating a new branch the git command would be similar to below:
2) git checkout -b FEAT_sunshine_api
For pushing a change to the remote branch the git command would be similar to the below commands:
<i>-git add .  <br/></i>
<i>-git commit -m “FEAT: Individual_walk”  <br/></i>
<i>-git push origin br_sunshine_api  <br/></i>
	
### Merging changes to the main branch
Pull requests let you tell others about changes you've pushed to a branch in a repository on GitHub. Once a pull request is opened, you can discuss and review the potential changes with collaborators and add follow-up commits before your changes are merged into the base branch.

#### Usage
1) Merging to the main branch should happen by each team member once per day
2) When a member of the team has completed work in their branch they should first initiate a pull from the remote main branch to their local main branch following the below commands (FEAT_sunshine_api represents the name of the branch that was created which contains the code changes): <br>
<i>-git add . <br/></i>
<i>-git commit -m “FEAT: Individual_walk”<br/></i>
<i>-git checkout main <br/></i>
<i>-git pull origin main <br/></i>
<i>-git checkout FEAT_sunshine_api <br/></i>
<i>-git merge main <br/></i>
<i>-git push origin FEAT_sunshine_api <br/></i>
3) Before merging the changes to the remote main branch you should initiate a pull request via the pull request menu at the top of the repository in GitHub.
4) Set the base branch as main
5) Set the compare branch to the branch containing the change
6) Press “Create pull request”
7) Specify a pull request name
8) Add a descriptive comment about each change you are wanting to merge
9) Assign two members in the team to the list of reviewers on the right hand side
10) Each reviewer will receive a notification to review the merge. Once each member in the team has performed a review and approve the final approver should perform the  action of merging the change by pressing the Merge button which will merge the changes to the main branch.

### Commit Messages
The commit command is used to save changes to a local repository after staging in Git.
The below naming convention and rules should be used when committing any change to any repository in this project:

1) The first word of the commit should be the keyword from the ‘Change Type’ list above in this document with capitalization of each character.
2) Followed by a semicolon and space character
3) Followed by a brief description of the change where the first character of the sentence is capitalized
4) Total length of the commit message is limited to 50 chars
5) Commits to the repository should be little and often

#### Usage
1) For fixing a bug in code the git command would be similar to below:  <br/>
<i>-git commit -m “FIX: Null Exception in Function GetWeather” </i>

### Branch Rules
1) A two step review and one step approval is required to merge from the remote branch to the remote repository.

## Coding Editors 
### IntelliJ
"# Backend-CSharp-API" 
"# Backend-CSharp-API" 
