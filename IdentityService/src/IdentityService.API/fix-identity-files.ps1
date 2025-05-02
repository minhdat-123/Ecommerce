# Get all .cshtml.cs files with only 3 lines (malformed files)
$files = Get-ChildItem -Path ".\Areas\Identity\Pages" -Recurse -Filter "*.cshtml.cs" | 
         Where-Object { 
             # Check if file has the formatting issue by looking at line count
             $content = Get-Content $_.FullName
             $content.Count -le 3 -and $content -match "ApplicationUser"
         }

Write-Host "Found $($files.Count) files to fix"

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    
    # Check if the using directive exists
    $hasUsingDirective = $content -match "using IdentityService.Domain.Entities;"
    
    # Split the content
    if ($content -match "using IdentityService.Domain.Entities;(.*)") {
        $contentAfterUsing = $Matches[1]
    } else {
        $contentAfterUsing = $content
    }
    
    # Format the code content - first split the parts
    if ($contentAfterUsing -match "// Licensed to the .NET Foundation(.*?)namespace (.*?) {(.*)}") {
        $licenseText = "// Licensed to the .NET Foundation" + $Matches[1]
        $namespace = $Matches[2]
        $classContent = $Matches[3]
    } else {
        Write-Host "Failed to match pattern in $($file.Name)"
        continue
    }
    
    # Reformat license part
    $licenseText = $licenseText -replace " // ", "`r`n// "
    # Add #nullable disable on its own line
    $licenseText = $licenseText -replace "#nullable disable", "`r`n#nullable disable`r`n"
    
    # Insert proper line breaks for using statements
    $licenseText = $licenseText -replace "using ", "`r`nusing "
    $licenseText = $licenseText -replace "using`r`n", "using "
    
    # Make sure the Domain.Entities namespace is included
    if (-not $hasUsingDirective) {
        $licenseText += "`r`nusing IdentityService.Domain.Entities;"
    }
    
    # Format class content by adding line breaks
    $classContent = $classContent -replace " {", "`r`n    {`r`n"
    $classContent = $classContent -replace "; ", ";`r`n        "
    $classContent = $classContent -replace " public ", "`r`n    public "
    $classContent = $classContent -replace " private ", "`r`n    private "
    $classContent = $classContent -replace " protected ", "`r`n    protected "
    $classContent = $classContent -replace " async ", "`r`n    async "
    $classContent = $classContent -replace "/// <summary>", "`r`n        /// <summary>"
    $classContent = $classContent -replace "///     ", "`r`n        ///     "
    $classContent = $classContent -replace "\[", "`r`n        ["
    $classContent = $classContent -replace "\[`r`n", "["
    
    # Reformat namespace part
    $newContent = $licenseText + "`r`n`r`nnamespace " + $namespace + "`r`n{" + $classContent + "`r`n}"
    
    # Write the formatted content back to the file
    $newContent | Set-Content $file.FullName
    
    Write-Host "Fixed: $($file.Name)"
}

Write-Host "Finished fixing files" 