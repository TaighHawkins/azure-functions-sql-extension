# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for license information.

using namespace System.Net

param($changes)

$expectedMaxBatchSize = $env:TEST_EXPECTED_MAX_BATCH_SIZE
if ($expectedMaxBatchSize -and $expectedMaxBatchSize -ne $changes.Count) {
    throw "Invalid max batch size, got $($changes.Count) changes but expected $expectedMaxBatchSize"
}

Write-Host "SQL Changes: $($changes.Count)"
foreach ($change in $changes) {
    # The output is used to inspect the trigger binding parameter in test methods.
    # Use -Compress to remove new lines and spaces for testing purposes.
    $changeJson = $change | ConvertTo-Json -Compress
    Write-Host "SQL Change: $changeJson"
}