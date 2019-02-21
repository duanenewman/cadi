param (
	[parameter(Position=0,
	   Mandatory=$false)]
	   [ValidateSet("Debug","Deploy")]
	   [string]$Action
)

if ($Action -eq ""){
	echo "No action specified, begining debug session"
	$Action = "Debug" 
}

if($Action -eq "Debug") {
	hugo server -v -w -D
} else {
	echo "deploying"
	rm -ErrorAction SilentlyContinue -r -force ../docs/*
	hugo -v -d ../docs/
	cp file.cname ../docs/CNAME
}