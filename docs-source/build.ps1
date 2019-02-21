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
	# rm -ErrorAction SilentlyContinue -r -force ../duanenewman.github.io/*
	hugo -v -d ../www/
	cp file.cname ../www/CNAME
}