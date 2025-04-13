# Get all Account folder .cshtml.cs files with only 3 lines (malformed files)
$files = Get-ChildItem -Path ".\Areas\Identity\Pages\Account" -Filter "*.cshtml.cs" | 
         Where-Object { 
             $content = Get-Content $_.FullName
             $content.Count -le 3 -and $content -match "ApplicationUser"
         }

Write-Host "Found $($files.Count) files to fix in Account folder"

foreach ($file in $files) {
    $templateContent = @"
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace IdentityService.API.Areas.Identity.Pages.Account
{
    public class CLASSNAME : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<CLASSNAME> _logger;

        public CLASSNAME(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<CLASSNAME> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // Your class-specific code will go here. This is a placeholder template.
        // You'll need to customize this for each file.
    }
}
"@

    $fileName = $file.Name
    $className = $fileName.Replace(".cshtml.cs", "Model")
    
    # Replace the placeholder with the actual class name
    $newContent = $templateContent.Replace("CLASSNAME", $className)
    
    # Write the formatted content back to the file
    $newContent | Set-Content $file.FullName
    
    Write-Host "Fixed: $($file.Name) with template structure."
}

Write-Host "Finished fixing files in Account folder" 