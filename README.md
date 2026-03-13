Encrypted password manager in C#

SYNOPSIS
<command> <args>
Arguments depend on what command you run and are defined in the
command-specific documentation. Your vaults (which contain your
passwords) are encrypted using a combination of your master password
and secret key.
<command> can be any of the following:
init <client path> <server path>
Create new vault.

create <client path> <server path>
Create a new client file (e.g., on another device) to an
already existing vault .

get <client path> <server path> <property> (if property not included, lists all properties)
Show stored values for some property or list properties
in vault.

set <client path> <server path> <property> 
Store value for some (possibly new) property in vault.

delete 
Drop some property from vault.

secret 
Show secret key.

change 
Change the master password
