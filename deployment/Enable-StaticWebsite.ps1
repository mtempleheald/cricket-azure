Param
(
    [string]$resourceGroup = "mthcricket01",
    [string]$storageAccountName = "mthcricket01uistorage"
)
az extension add --name storage-preview
az storage blob service-properties update --account-name "$storageAccountName" --static-website --404-document 404.html --index-document index.html