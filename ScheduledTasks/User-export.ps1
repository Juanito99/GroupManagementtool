<#
.Synopsis
Create a text file which will be queried for the user selection.
To have most accurate data, schedule execution regular, e.g. every 4 hours
.Description
Create a text file which will be queried for the user selection.
To have most accurate data, schedule execution regular, e.g. every 4 hours
.Example
User-export.ps1
.Notes
Need configuration in the Script configuration section,
AUTHOR: Ruben
LASTEDIT: 2015-04-01
REQUIRES: PowerShell Version 2, Active Directory Managment tools installed.
NAME: Users-export.ps1
KEYWORDS: ActiveDirectory, Users, Regular Expressions
REMARK: 
This PS script comes with ABSOLUTELY NO WARRANTY; for details
see gnu-gpl. This is free software, and you are welcome
to redistribute it under certain conditions; see gnu-gpl for details.
#>

# -----------------------</ Script Configuration \>--------------------
	$ErrorActionPreference = "Continue"
    #requires -version 2.0
	Set-StrictMode -Version Latest        
    $outputFile = "D:\GroupManagementTool\Appdata\ADUsers.txt" #Change here to fit to your website location!
	$ingoreOUs = "(.*OU=Administrative.*|.*OU=Services.*|CN=Builtin.*|CN=Users*)"   #Avoid to export users from specific OUs.
    $error.Clear()     
# -----------------------<\ Script Configuration />---------------------  

Import-Module ActiveDirectory

$allusrs = Get-ADUser -Filter {(Surname -Like "*") -and  (enabled -eq $true)} -Properties sn, givenName, samAccountName -SearchScope Subtree | Select-Object sn, givenName, samAccountName

$filteredusrs = New-Object System.Collections.Generic.List[string]

$allusrs | ForEach-Object {
    $dn = $_.DistinguishedName 
    if ($dn -notmatch $ingoreOUs) {
        $filteredusrs.Add("$($_.sn), $($_.givenName); $($_.samAccountName)") | Out-Null
    }        
}

$filteredusrs | Out-File $outputFile
