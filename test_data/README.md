# Test data

Fixture CSVs for manual testing and TestComplete automation (which needs real files on disk, not generated ones).

- `good_linear.csv` — well-formed, two numeric columns with a roughly linear relationship. The happy-path fixture.
- `mixed_columns.csv` — includes a non-numeric `label` column alongside numeric ones; only `x`/`y` should be selectable.
- `malformed_row.csv` — one row is missing its trailing value; should load with a blank cell, not crash.
- `empty.csv` — zero bytes; should trigger the "invalid file" warning dialog, not a silent blank grid.
