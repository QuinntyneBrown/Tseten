import { test, expect } from '@playwright/test';

test.describe('SpecSync App', () => {
  test('should display application title', async ({ page }) => {
    await page.goto('/');

    // The app should load successfully
    await expect(page).toHaveTitle(/SpecSync/);
  });

  test('should have router outlet', async ({ page }) => {
    await page.goto('/');

    // The router outlet should be present
    const routerOutlet = page.locator('router-outlet');
    await expect(routerOutlet).toBeAttached();
  });

  test('should render without errors', async ({ page }) => {
    // Listen for console errors
    const errors: string[] = [];
    page.on('console', msg => {
      if (msg.type() === 'error') {
        errors.push(msg.text());
      }
    });

    await page.goto('/');

    // Wait for the page to be fully loaded
    await page.waitForLoadState('networkidle');

    // Check that no critical errors occurred
    const criticalErrors = errors.filter(e =>
      !e.includes('favicon') &&
      !e.includes('404')
    );
    expect(criticalErrors).toHaveLength(0);
  });

  test('should be responsive', async ({ page }) => {
    await page.goto('/');

    // Test mobile viewport
    await page.setViewportSize({ width: 375, height: 667 });
    await expect(page.locator('body')).toBeVisible();

    // Test tablet viewport
    await page.setViewportSize({ width: 768, height: 1024 });
    await expect(page.locator('body')).toBeVisible();

    // Test desktop viewport
    await page.setViewportSize({ width: 1920, height: 1080 });
    await expect(page.locator('body')).toBeVisible();
  });
});
