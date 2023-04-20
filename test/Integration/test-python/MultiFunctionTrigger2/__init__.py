# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.

import json
import logging

def main(changes):
    changes = json.loads(changes)
    for change in changes:
        # The output is used to inspect the trigger binding parameter in test methods.
        logging.info("Trigger2 Change: %s", change)
