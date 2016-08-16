param($installPath, $toolsPath, $package, $project)

$pinvokePath = Join-Path $installPath "lib\native\librets-pinvoke.dll"

write-host $pinvokePath

$retsInvoke = $project.ProjectItems.AddFromFile($pinvokePath)
$retsInvoke.Properties.Item("CopyToOutputDirectory").Value = 1
$retsInvoke.Properties.Item("BuildAction").Value = 0