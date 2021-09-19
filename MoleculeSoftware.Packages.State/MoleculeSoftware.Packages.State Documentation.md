# MoleculeSoftware.Packages.State

## Documentation

### What is State

State is a simple and robust caching system for MoleculeSoftware software packages. It utilizes a SQLite database to create a pseudo NO-SQL data store which accepts simple commands to perform storage, update, delete, retrieval, and purge operations.  

### Commands 

The state manager creates a database that can store and retrieve key/value pairs. 

Commands are passed into the state manager using the MoleculeStoate object's DatabaseOperation method.

The database operation method will provide a callback for all commands

The command structure is as follows:

### The initiator

The initiator is the first part of the command string which instructs State to perform a particular function. We will begin command strings in several ways

* STORE#> - instructs State to store a new value
* RETRIEVE#> - Retrieves a value
* UPDATE#> - Updates a value
* DELETE#> - Deletes a value
* PURGE#> - Purges all values

### Command parameters

The command parameter is the second part of the command string. This parameter typically contains the name of the key, but is also uded as a command verifier for the purge command
The initiator will be combined with a command parameter as follows:

* STORE#> will accept a key as a the first parameter. This command will appear as: STORE#>KEY_NAME
* RETRIEVE#> will accept a key as the first parameter. This command will appear as: RETRIEVE#>KEY_NAME
* UPDATE#> Will accept a key as the first parameter. This command will appear as: UPDATE#>KEY_NAME
* DELETE#> will accept a key as the first parameter. This command will appear as: DELETE#>KEY_NAME
* PURGE#> will accept a verifier as the first parameter. This command will appear as: PURGE#>1 - Note that unless the verifier is passed, purge will not occur

### Value Parameters

Value parameters follow the command parameter and will contain the value that you want to store/update
Only the STORE and UPDATE commands require a value parameter

STORE#>KEY_NAME#>VALUE
UPDATE#>KEY_NAME#>VALUE

VALUE = String representation of the value you wish to store

### Data Types

Data is stored by State in string form. The user will be required to convert data to his/her preferred data type once retrieved from State

### Storing values

Storing a value is accomplished by passing a value parameter into the STORE#>KEY_NAME#> command

This operation will appear as: STORE#>KEY_NAME#>VALUE

With KEY_NAME and VALUE represented by your key and value data

### Updating Values

Updating a value is accomplished by passing a value parameter into the UPDATE#>KEY_NAME#> command

This operation will appear as: UPDATE#>KEY_NAME#>NEW_VALUE

With KEY_NAME and VALUE represented by your key and value data

### Retrieving Values

Retrieving a value is accomplished by passing the key name as a command parameter into the RETRIEVE#> command

This operation will appear as: RETRIEVE#>KEY_NAME

With KEY_NAME represented by your key data

### Deleting Values

Deleting a value is accomplished by passing the key name as a command parameter into the DELETE#> command

This operation will appear as: DELETE#>KEY_NAMEE

With KEY_NAME represented by your key data

### Purging the database of all values

Purging the database is accomplished by passing the PURGE#> command with a command verifier as the command parameter

This operation will appear as PURGE#>1

Unless 1 is passed as the command parameter, the purge operation will not complete

### Callbacks

Callbacks are 

* #OK# is returned after storing a value in the database, performing a successful delete operation, or performing a successful update operation

* #NONE# is returned if no record was located or if there was an error performing the particular command

* VALUE is returned if a record was located when performing the RETRIEVE#> command (Value is the value that will be returned in string form)
  * (please note that you will need to perform a conversion of the data if it is not ultimately to be used as a string.)

## Setup

Install the nuget package MoleculeSoftware.Packages.State

## Application Startup 

When your application that will be using State has started, create a MoleculeState object and call the Init() method. This will delete any existing state database and will perform any migrations that are necessary to set up the database file. The database file is stored in the temporary directory using the name MoelculeStateData.db

NOTE: If Init() is not called the database will not be created. 

NOTE: If Init() is called elsewhere, the state database will be deleted and recreated. This is not recommended to be used as a purge since database migrations must be applied every time it is called. Although it will work, this can be slower than using the inbuilt PURGE#> command. 

## Using State

Create a MoleculeState object and call the DatabaseOperation method. Pass the command string as a parameter. This method will return the string result. See the callbacks section for possible results returned. 

## Cleanup

State will clean up after itself during each application startup. If you wish to delete the state data at application exit, just delete the MoleculeStateData.db file from the user's temporary director. 

## Contribution

If you wish to contribute to this library, please feel free. This was a quick project but has been a very valuable state storage system for MoleculeSoftware applications. Adaptations and updates are welcome. 