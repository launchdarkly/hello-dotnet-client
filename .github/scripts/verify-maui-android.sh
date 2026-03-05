#!/bin/bash
set -e

echo "Installing APK..."
adb install MauiApp/bin/Debug/net8.0-android/com.launchdarkly.hello-Signed.apk

echo "Clearing logcat..."
adb logcat -c

echo "Launching app..."
adb shell monkey -p com.launchdarkly.hello -c android.intent.category.LAUNCHER 1

for i in 1 2 3 4; do
  echo "Waiting 15 seconds (attempt $i of 4)..."
  sleep 15

  echo "=== Checking if app is running ==="
  adb shell pidof com.launchdarkly.hello || echo "App process NOT running"

  echo "=== Logcat (crash/mono related) ==="
  adb logcat -d -t 100 2>&1 | grep -iE "AndroidRuntime|FATAL|crash|mono|dotnet|HelloDotNet|launchdarkly" | tail -50 || true

  echo "=== UI dump ==="
  adb exec-out uiautomator dump /dev/tty > /tmp/uidump.xml 2>&1 || true
  cat /tmp/uidump.xml

  if grep -q 'feature flag evaluates to' /tmp/uidump.xml; then
    echo "SUCCESS: Flag evaluation verified"
    exit 0
  fi
  echo "Attempt $i: text not found yet, retrying..."

  # If app crashed, try relaunching
  if ! adb shell pidof com.launchdarkly.hello > /dev/null 2>&1; then
    echo "App not running, relaunching..."
    adb shell monkey -p com.launchdarkly.hello -c android.intent.category.LAUNCHER 1
  fi
done

echo "=== FULL LOGCAT DUMP (last 200 lines) ==="
adb logcat -d 2>&1 | tail -200 || true

echo "FAILURE: Could not verify flag evaluation after 4 attempts"
exit 1
