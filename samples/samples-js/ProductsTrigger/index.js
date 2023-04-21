// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

module.exports = async function (context, changes) {
    context.log(`SQL Changes: ${changes.length}`)
    for (var change of changes) {
        // The output is used to inspect the trigger binding parameter in test methods.
        context.log(`SQL Change: ${JSON.stringify(change)}`)
    }
}