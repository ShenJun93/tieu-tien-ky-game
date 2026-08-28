# ttk-mobile-performance-budget

## WHEN TO USE

A player-facing mobile slice changes rendering, VFX, animation, UI, AI load, scene content, memory/load behavior, or claims target-device readiness.

## PRODUCT QUESTION

Can the representative experience sustain its intended responsiveness on the target device class without unstable frame pacing, thermal collapse, excessive memory/load cost or input-latency degradation?

## MUST

- Declare task-specific performance targets before claiming readiness: target FPS/frame-time budget, representative device tier(s), test scene/encounter and session duration appropriate to the claim.
- Measure on physical target devices when `product_gate.target_device_required=true`; Editor/desktop profiling cannot certify mobile readiness.
- Prefer frame-time stability over peak FPS. Record long/slow-frame behavior when material.
- Consider CPU/GPU bound state, memory, loading, temperature/thermal throttling and touch-to-display responsiveness where the slice can affect them.
- Profile representative combat density, not an empty arena.
- Re-measure after optimization; do not infer improvement from code changes alone.
- Use Unity Profiler/Profile Analyzer and platform tools such as Perfetto/AGI when the question requires deeper diagnosis.

## MUST NOT

- Hard-code one universal FPS target for every device/task without a product decision.
- Call `android_build=PASS` a performance PASS.
- Benchmark only a cold, short or unrepresentative scene when the claim concerns sustained combat.
- Sacrifice critical UI legibility/touch targets merely to hit a metric.

## EXIT CONDITION

`target_device_readiness=PASS` means the task's declared budget was actually measured on representative hardware and no known performance/thermal/input issue invalidates the Human product question. It is readiness evidence, not FEELS/BELONGS acceptance.

## External basis

Current Android game guidance emphasizes measuring FPS/frame times, CPU/GPU bottlenecks, thermal behavior, frame pacing and target-device variation; Unity mobile guidance likewise recommends explicit per-frame budgets and physical-device profiling.