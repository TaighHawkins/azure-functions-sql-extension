# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.

import json
import logging
import os

def main(changes):
    expectedMaxBatchSize = os.environ.get("TEST_EXPECTED_MAX_BATCH_SIZE")
    length = len(json.loads(changes))
    if expectedMaxBatchSize and int(expectedMaxBatchSize) != length:
        raise Exception("Invalid max batch size, got %d changes but expected %s" % (length, expectedMaxBatchSize))
    changes = json.loads(changes)
    logging.info("SQL Changes: %d", length)
    for change in changes:
        # The output is used to inspect the trigger binding parameter in test methods.
        logging.info("SQL Change: %s", change)
