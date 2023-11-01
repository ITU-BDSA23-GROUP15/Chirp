# B2C-tenant

## Why did we choose this OAuth-method?
We chose to use B2C-tenant as our OAuth-method. 
This is mainly due to us thinking it is the thing mostly used in the industry, and to let middleware handle the security and login-part of the service while allowing us to extract information.

### Pros
- Let middleware handle the authentication
- We don't have to maintain (and create) a database
- Easy access to obtain user-information
- Good UI

### Cons
- Hard to configure properly
- Less control over the database and code behind
