# CHANGELOG

All notable changes to this project will be documented in this file.
This project adheres to [Semantic Versioning](http://semver.org/) and [Keep a Changelog](http://keepachangelog.com/).

## Unreleased

---

### New

### Changes

- Display blueprints and frames on air pipe overlays. (#4)
- Show the air pipe overlay when a building is selected. (#3)

### Fixes

- Buildings are not blocked by furniture anymore. (#2)

### Breaks

## 2.0.0 - (2021-08-29)

---

### New

- The power consumption of industrial buildings depends on their load.
- Intake fans and temperature control units now have a dynamic load, that takes some times to adjust to the needs.
- Wall-mounted intake fan.

### Changes

- The global throughput of an air network is limited by both its intake fans and its vents.
- Building cost, power consumption and capacity of all buildinds have been reworked.
- Intake fans and temperature control units can be switched off.
- Buildings can work with a partially blocked area, at the cost of lower performances.
- Regular intake fans only needs 4 free tiles around them instead of 8.

### Fixes

- Do not exhaust frozen air when a temperature control unit is broken.

### Breaks

- Only support Rimworld 1.3+.
- Internals have been rewritten from scratch.
- Do not support buildings of CCC version 1.5 or previous.
- Because of reworking, translations have been dropped, sorry.

## 1.0.20.0 - (2021-07-23)

---

Rebuild on the 1.3 binaries as some functions changed from the beta

## 1.0.19.0 - (2021-07-08)

---

Mod updated for 1.3 and passed autotests

## 1.0.18.0 - (2021-01-05)

---

Added the amount of air pushed in the vents description, thanks Andy!

## 1.0.17.0 - (2020-11-05)

---

Cleaner graphics again

## 1.0.16.0 - (2020-11-04)

---

Updated graphics, corrected labels, buildnings in temperature-tab instead of its own

## 1.0.15.0 - (2020-11-02)

---

Cleaner graphics by spoden

## 1.0.14.0 - (2020-09-06)

---

Moved old stuff to 1.0 folder to fix a few errors

## 1.0.13.0 - (2020-08-15)

---

Fixed manifest and about-file

## 1.0.12.0 - (2020-08-01)

---

Updated for 1.2
