import http from 'k6/http';
import { check } from 'k6';
import { Rate } from 'k6/metrics';

const checkFailureRate = new Rate('check_failure_rate');

export let options = {
  summaryTrendStats: ['avg', 'p(95)'],
  stages: [
    // Linearly ramp up from 1 to 50 VUs during 10 seconds
    { target: 50, duration: '10s' },

    // Hold at 50 VUs for the next 1 minute
    { target: 50, duration: '1m' },

    // Linearly ramp down from 50 to 0 VUs over the last 15 seconds
    { target: 0, duration: '15s' },
  ],

  thresholds: {
    // We want the 95th percentile of all HTTP request durations to be less than 500ms
    http_req_duration: ['p(95)<500'],

    // Thresholds based on the custom metric we defined and use to track application failures
    check_failure_rate: [
      // Global failure rate should be less than 1%
      'rate<0.01',

      // Abort the test early if it climbs over 5%
      { threshold: 'rate<=0.05', abortOnFail: true },
    ],
  },
};

export default function () {
  // execute http get call
  let response = http.get('https://localhost:5001/api/internal-employees');

  // check() returns false if any of the specified conditions fail
  const result = check(response, {
    'status is 200': (r) => r.status === 200,
  });

  // We reverse the check() result since we want to count the failures
  checkFailureRate.add(!result);
}
