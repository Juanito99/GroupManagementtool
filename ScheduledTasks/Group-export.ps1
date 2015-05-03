<#
.Synopsis
Exports all groups in Active Directory where the 'managedBy' attribute is Set
and the managedBy accounts has write permissions to the group object.
.Description
Exports all groups in Active Directory where the 'managedBy' attribute is Set
and the managedBy accounts has write permissions to the group object.
.Example
Group-export.ps1
.Notes
Need configuration in the Script configuration section.
AUTHOR: Ruben
LASTEDIT: 2015-04-01
REQUIRES: PowerShell Version 2, Active Directory Managment tools installed.
NAME: Group-export.ps1
KEYWORDS: ActiveDirectory, Groups, ManagedBy
REMARK: 
This PS script comes with ABSOLUTELY NO WARRANTY; for details
see gnu-gpl. This is free software, and you are welcome
to redistribute it under certain conditions; see gnu-gpl for details.
#>

# -----------------------</ Script Configuration \>--------------------
	$ErrorActionPreference = "Continue"
    #requires -version 2.0
	Set-StrictMode -Version Latest        
    $error.Clear()     
	$outputFile = "D:\GroupManagementTool\Appdata\ADGroups.txt" #Change here to fit to your website location!
    $domain = "nwtraders.com" #Change  here to your domain name!
	#Below 2 lines are optional, in case some OUs or domains shall be skipped during export.
	$ingoreOUs = "(.*OU=Administrative.*|CN=Builtin.*|CN=Users*)"
	$ignoreStr = "(ROOTDomain\\)|(S-1-5-21.{38})"     
# -----------------------<\ Script Configuration />---------------------   

Import-Module ActiveDirectory
$allgrps = Get-ADGroup -LDAPFilter "(managedBy=*)" | Select-Object DistinguishedName

$membersObj = "bf9679c0-0de6-11d0-a285-00aa003049e2"
$groups = @{}

$allgrps | ForEach-Object {
    $grpdn = $_.DistinguishedName 
    if ($grpdn -notmatch $ingoreOUs) {
        $modACEo = (Get-Acl "AD:\$grpdn").Access | Where-Object { ($_.ActiveDirectoryRights -eq "WriteProperty") -and ($_.ObjectType -eq $membersObj) } | Select-object IdentityReference                      
        $modACEs = $modACEo.IdentityReference -notmatch $ignoreStr        
        if ($modACEs) {
            $groups.Add($grpdn,$modACEs) | Out-Null
            $modACEo = $null
            $modACEs = $null
        }        
    }
}

$alp = New-Object System.Collections.ArrayList

foreach ($pair in $groups.GetEnumerator()) {       
    $alp.add("$($pair.Name)#$($pair.Value)") | Out-Null
}

$all = New-Object System.Collections.ArrayList

$alp | ForEach-Object {
    $itms = $_ -split '#'
    $tmpa = $itms[0] -replace "$domain\\",""
    $tmpb = $itms[1] -split "(?<=$domain\\)(?=.)" -join '|'
    $tmpb = ($tmpb -replace "$domain\\","").Trim()
    $tmpb = $tmpb.Remove(0,1)
    $tmpb = $tmpb -replace " |",""    
    $all.add("$tmpa#$tmpb") | Out-Null
}

$all | Out-File $outputFile