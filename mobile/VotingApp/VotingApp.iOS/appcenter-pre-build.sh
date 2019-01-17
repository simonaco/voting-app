#!/usr/bin/env bash
SyncfusionConstantsFile=`find "$APPCENTER_SOURCE_DIRECTORY" -name SyncfusionConstants.cs | head -1`

echo SyncfusionConstantsFile = $SyncfusionConstantsFile

sed -i '' "s/Your License Key Here/$SyncFusionLicenseKey/g" "$SyncfusionConstantsFile"
sed -i '' "s/#warning/\/\/#warning/g" "$SyncfusionConstantsFile"

echo "Finished Injecting SyncFusion License Key"



