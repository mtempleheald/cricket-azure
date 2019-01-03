Param
(
    [string]$resourceGroup = "mthcricket01",
    [string]$storageAccountName = "mthcricket01uistorage"
)

$storageAccountKey = Get-AzStorageAccountKey -ResourceGroupName $resourceGroup -Name $storageAccountName | Where-Object { $_.KeyName -eq 'key1' } | Select-Object -ExpandProperty Value
$storageAccountContext = New-AzStorageContext -StorageAccountName "$storageAccountName" -StorageAccountKey "$storageAccountKey"
Enable-AzStorageStaticWebsite -IndexDocument index.html -ErrorDocument404Path 404.html -Context $storageAccountContext
