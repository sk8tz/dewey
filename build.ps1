param(
    $a
)

$src = dir .\src

function build {
    foreach ($dir in $src) {
        Set-Location .\src\$dir
        dotnet build
        Set-Location ..\..\
    }
}

function pack {
    foreach ($dir in $src) {
        Set-Location .\src\$dir
        dotnet pack
        Set-Location ..\..\
    }
}

function restore {
    foreach ($dir in $src) {
        Set-Location .\src\$dir
        dotnet restore
        Set-Location ..\..\
    }
}

function publish {
    foreach ($dir in $src) {
        Set-Location .\src\$dir\bin\debug

        $packages = Get-ChildItem -Filter *.nupkg | Where-Object { $_.Extension -eq '.nupkg' }

        foreach($package in $packages) {
            nuget push $package
        }

        Set-Location ..\..\..\..\
    }
}

function love {
    "Awww, we love you too!"
}

switch($a) {
    "restore" {
        restore
    }
    "build" {
        build
    }
    "pack" {
        pack
    }
    "publish" {
        publish
    }
    "love" {
        love
    }
    default { 
        restore
        build
        pack
        ""
        "---------------------------------------"
        ""
        "NOTE: You must run `publish` manually."
        ""
        "---------------------------------------"
        ""
    }
}