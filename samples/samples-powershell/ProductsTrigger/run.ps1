# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for license information.

using namespace System.Net

param($changes)
foreach ($change in $changes) {
    # The output is used to inspect the trigger binding parameter in test methods.
    # Use -Compress to remove new lines and spaces for testing purposes.
    $changeJson = $change | ConvertTo-Json -Compress
    Write-Host "SQL Change: $changeJson"
}