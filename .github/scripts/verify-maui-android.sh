#!/bin/bash
set -e

echo "Installing APK..."
adb install MauiApp/bin/Debug/net8.0-android/com.launchdarkly.hello-Signed.apk

echo "Launching app..."
adb shell monkey -p com.launchdarkly.hello -c android.intent.category.LAUNCHER 1

for i in 1 2 3 4; do
  echo "Waiting 15 seconds (attempt $i of 4)..."
  sleep 15
  adb exec-out uiautomator dump /dev/tty > /tmp/uidump.xml 2>&1 || true
  echo "UI dump contents:"
  cat /tmp/uidump.xml
  if grep -q 'feature flag evaluates to true' /tmp/uidump.xml; then
    echo "SUCCESS: Flag evaluation verified"
    exit 0
  fi
  echo "Attempt $i: text not found yet, retrying..."
done

echo "FAILURE: Could not verify flag evaluation after 4 attempts"
exit 1
