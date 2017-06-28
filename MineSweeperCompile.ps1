#File Paths
$cscPath = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"
$folderFiles = "C:\Users\chriale\Desktop\CSharp\Minesweeper\"
$iconPath = "C:\Users\chriale\Desktop\CSharp\Minesweeper\MineIcon.Ico"

#start building the compile string
$compile = "$($cscPath) "

#add the parameters
$compile += "/win32icon:$($iconPath) /res:$($iconPath) /target:winexe /out:$($folderFiles)Minesweeper.exe "

#add the Source Files
$compile += "$($folderFiles)Main.cs "
$compile += "$($folderFiles)Cell.cs "
$compile += "$($folderFiles)Board.cs "
$compile += "$($folderFiles)GameGUI.cs "
$compile += "$($folderFiles)Exceptions.cs "
$compile += "$($folderFiles)GameTimer.cs "
$compile += "$($folderFiles)CustomEventArgs.cs "
$compile += "$($folderFiles)OptionsMenu.cs "
$compile += "$($folderFiles)OptionsMenuData.cs "
$compile += "$($folderFiles)AboutMenu.cs "
$compile += "$($folderFiles)GameResources.cs "
$compile += "$($folderFiles)GameStatisticsManager.cs "
$compile += "$($folderFiles)GameStatisticData.cs "
$compile += "$($folderFiles)CustomFormClasses.cs"

Write-Host "$($compile)`n" -F Yellow
Invoke-Expression $compile

Write-Host "Finished! " -F Green -NoNewline
Write-Host "Press Any Key to Exit:" -F Cyan -NoNewline
Read-Host