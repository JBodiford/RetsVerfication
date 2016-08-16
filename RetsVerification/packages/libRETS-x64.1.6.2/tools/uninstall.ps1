param($installPath, $toolsPath, $package, $project)

$retsInvoke = $project.ProjectItems.Item("librets-pinvoke.dll")
$retsInvoke.Remove()